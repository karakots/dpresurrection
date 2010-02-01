<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Campaigns.aspx.cs" Inherits="Campaigns" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage My Campaigns</title>
    <link rel="stylesheet" type="text/css" href="~/css/Campaigns.css" />     
    <script type="text/javascript" src="Campaigns.js"></script>
</head>
<body id="PageBody"  runat="server">
    <form id="form1" runat="server">
    <input type="hidden" ID="SortColumnName" name="SortColumnName" runat="server" />
    <input type="hidden" ID="ExpandedItemIDs" name="ExpandedItemIDs" runat="server" />
    <input type="hidden" ID="newPlanName" name="newPlanName" runat="server" />
    <input type="hidden" ID="newCampaignName" name="newCampaignName" runat="server" />
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
        <table ID="ContentTable" cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="InfoDiv" runat="server" >
               <h1>Manage My Campaigns</h1><p>Manage your campaign or their associated media plans using the action buttons to view, edit, analyze or create an order list.
               
                   </p>
</td>
               <td id="RHCol" runat="server" rowspan="3" style="z-index:1000">
               <div id="RHDiv"></div>
               <div style="text-align:center"> 
                <asp:Button runat="server" ID="NewCampaignButton" 
                       onclick="CreateCampaign_Click" Text="Create New Campaign" BackColor="#FF9900" 
                       Font-Bold="True" Width="164px" Height="28px" style="margin-left: 4px"/><br />
                       </div>
                       
                       <div runat="server" id="RHInfoArea" style="color:#004;font-size:10pt;padding-left:0px;width:160px;text-align:center;"></div>
                   <div id="Col2VideoDiv1"><br /><a href="#" onclick="OpenDemoWindow();" target="_blank">How to Plan<br />Using AdPlanit</a><br /><span class="vidLabel">video</span></div>
                   <div ><a  id="AdminLink" runat="server" href="admin/Home.aspx">User Admin</a></div>
               </td>
           </tr>
           <tr>
               <td ID="CampaignsListDiv" runat="server">&nbsp; </td>
           </tr>
            <tr>
                <td id="FooterCell" align="center">
                    <table id="FooterTable" align="center">
                        <tr>
                            <td id="Footer">
                                <%= WebLibrary.Utils.Footer %>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="position: relative; top: -200px; left: 425px; z-index:10">
                        <asp:Panel ID="VisitorInfo" runat="server" BackColor="#0066FF" BorderStyle="Solid"
                            BorderWidth="2px" Style="margin: 10px; padding: 10px; height: 101px; width: 284px;" EnableViewState="False">
                            <div style="text-align: center; height: 100%;">
                                <b>Welcome to the AdPlanit Tour.
                                    <br />
                                    <br />
                                    Click on the plan name to view media details. </b>
                            </div>
                        </asp:Panel>
                    </div>
                </td>
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
            <p id="PopupCaptionTextP" class="PopupCaptionTextP" runat="server" >Enter name for the new plan:</p> 
            <input type="text" id="NewNameBox"  class="NewNameBoxShown" onkeypress="return CheckForEnter(event);" runat="server" /> <br />
            <span id="AutoGenSpan" runat="server" > <asp:CheckBox runat="server" Text="Autogenerate plan contents" ID="CreatePlanUseAuto" /></span>
            <table cellpadding="0" cellspacing="0" id="PopupButtonsTbl" class="PopupButtonsTable" ><tr><td>
            <a id="hideModalPopupViaClientButton" runat="server" href="#" ><img src="images/Button-OK.gif" border="0"  /></a>
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
