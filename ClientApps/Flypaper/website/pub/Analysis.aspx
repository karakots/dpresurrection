<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Analysis.aspx.cs" Inherits="Analysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit Media Plan Analysis</title>
    <link rel="stylesheet" type="text/css" href="~/css/Analysis.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" ID="ExpandedItemTypes" name="ExpandedItemTypes" runat="server" />
    <input type="hidden" ID="newPlanName" name="newPlanName" runat="server" />
    <input type="hidden" ID="expandSummary" name="expandSummary" value="true" runat="server" />
    <input type="hidden" ID="expandDetails" name="expandDetails" value="false" runat="server" />
    <input type="hidden" ID="expandSuggestions" name="expandDetails" value="false" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="300" />    
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
                        <li><a href="HQ.aspx" id="A3" class="nav_shop" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                        <li><a href="Order.aspx" id="A4" class="nav_order" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                        <li><a href="#" id="A5" class="nav_analyze active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
                    </ul>
                    </div>                   
        
                <asp:UpdatePanel runat="server" ID="UpdatePnl1" UpdateMode="Always" >
                <ContentTemplate>
                    <table id="InnerTable" cellpadding="2" cellspacing="0">
                    <tr><td colspan="4" ID="SummaryHeader" runat="server" style="width: 600px; padding-top: 70px; font-size: 12pt; font-weight: normal; color:#4477CC; border-bottom: solid 2px #8fC2F4;">
                         Performance by Segment <%= SummaryExpandImage() %>
                         </td>
                    </tr>
                    <tr>
                        <td colspan="4" ID="SelectionCell" runat="server" style="padding-top: 10px;">Segment: <asp:DropDownList runat="server" ID="ChartSelection" AutoPostBack="true">
                        <asp:ListItem Text="Test1" Value="test1"></asp:ListItem>
                        </asp:DropDownList>&nbsp;&nbsp;<asp:Checkbox runat="server" ID="ShowAllMetrics" Text="Show All Metrics" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" ID="ResultsChartCell" runat="server"><%= ChartImageTag() %></td>
                    </tr>                    
                    <tr>
                        <td colspan="4" ID="ResultsSummaryCell" runat="server">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" ID="DetailsHeader" runat="server" style="width: 600px; font-size: 12pt; font-weight: normal; color:#4477CC; border-bottom: solid 2px #8fC2F4;" >Detailed Results <%= DetailsExpandImage() %></td>
                    </tr>                    
                    
                        <tr id="GraphSelectionRow" runat="server" style="min-width:600px;width: 600px; ">
                          <td ID="SelectionCell2" runat="server" style="width: 70px;max-width: 70px;text-align:right; margin-right: 5px;">Segment:</td>
                          <td><asp:DropDownList runat="server" ID="GraphSelection" AutoPostBack="true">
                            <asp:ListItem Text="Test1" Value="test1"></asp:ListItem>
                          </asp:DropDownList></td>
                        <td  ID="SelectionCell3" runat="server" style="width: 110px;max-width: 110px;text-align:right;" >
                        Metric:</td>
                        <td><img src="images/Legend1.gif" style="position:relative; top:2px; margin-right:3px" /><asp:DropDownList runat="server" ID="GraphMetric" AutoPostBack="true">
                            <asp:ListItem Text="Test1" Value="test1"></asp:ListItem>
                        </asp:DropDownList>
                        </td>
                      </tr>
                      <tr id="GraphSelectionRow2" runat="server"  style="min-width:600px;width: 600px; ">
                        <td  ID="SelectionCell4" style="width: 70px;max-width: 70px;text-align:right;">Region:</td>
                        <td><asp:DropDownList runat="server" ID="GraphRegion" AutoPostBack="true">
                           <asp:ListItem Text="Test1" Value="test1"></asp:ListItem>
                        </asp:DropDownList>
                        </td>

                        <td  id="CompareLabel" runat="server" style="width: 110px;max-width: 110px; text-align:right;margin-left:23px;">
                        Compare&nbsp;Plan:</td>
                        <td><img id="CompareLegend" runat="server" src="images/Legend2.gif" style="position:relative; top:2px; margin-right:3px" /><asp:DropDownList runat="server" ID="ComparisonPlans" AutoPostBack="true">
                           <asp:ListItem Text="None" Value="" Selected="True"></asp:ListItem>
                        </asp:DropDownList></td>
                    </tr>
                    </table>
                   <table id="InnerTable2" cellpadding="2" cellspacing="0">
                    <tr>
                        <td colspan="4" ID="ResultsGraphCell" runat="server"><%= GraphImageTag() %></td>
                    </tr>  
                        <tr><td colspan="4" ID="SuggestionsHeader" runat="server" style="width: 600px; padding-top: 40px; font-size: 12pt; font-weight: normal; color:#4477CC; border-bottom: solid 2px #8fC2F4;">
                         Improvement Suggestions <%= SuggestionsExpandImage() %>
                         </td>
                    </tr>
                         <tr id="SuggestionSelectionRow" runat="server" style="min-width:600px;width: 600px; ">
                             <td ID="SelectionCell6a" runat="server" style="width: 30px;max-width: 30px;text-align:left; padding-left: 5px; padding-top: 2px;">
                                Plan:
                            </td>
                            <td ID="SelectionCell6" runat="server" style="width: 230px;max-width: 230px;text-align:left; margin-left: 15px;">
                               <asp:RadioButton runat="server" ID="ModifyExisting" GroupName="CreateOrMod" Text="Modify Existing" Checked="true"  Enabled="true"/>
                            </td>
                               <td ID="SelectionCell7" colspan="1" runat="server" style="width: 370px;max-width: 370px;text-align:left; margin-left: 15px; visibility:hidden;">
                                <asp:RadioButton runat="server" ID="HoldBudget" GroupName="NewBudget" Text="Maintain Current Budget" Checked="true" />
                            </td>
                           </tr>
                           <tr id="SuggestionSelectionRow2" runat="server" style="min-width:600px;width: 600px; ">
                           <td ID="Td1" runat="server" style="width: 30px;max-width: 30px;text-align:left; padding-left: 5px;">
                                &nbsp;
                            </td>
                            <td ID="SelectionCell8" runat="server" style="width: 230px;max-width: 230px;text-align:left; margin-left: 15px;">
                                 <asp:RadioButton runat="server" ID="CreateNew" GroupName="CreateOrMod" Text="Create New" />
                            </td>
                               <td ID="SelectionCell9" colspan="1" runat="server" style="width: 370px;max-width: 370px;text-align:left; margin-left: 15px; visibility:hidden;">
                                <asp:RadioButton runat="server" ID="SetBudget" GroupName="NewBudget" Text="New Budget: $&nbsp;" /><asp:TextBox runat="server" ID="NewBudgetTextBox" Text="" />
                            </td>
                           </tr>
                           <tr id="SuggestionSelectionRow3" runat="server" style="min-width:600px;width: 600px; ">
                             <td ID="Td2" runat="server" colspan="4" style="text-align:left; margin-left: 15px;">
                                Metric to Improve:&nbsp;<asp:DropDownList runat="server" ID="ImprovMetric">
                                <asp:ListItem Value="ReachAndAwareness">Reach and Awareness</asp:ListItem>
                                <asp:ListItem Value="Recency" >Recency</asp:ListItem>
                                <asp:ListItem Value="Persuasion" >Persuasion</asp:ListItem>
                                <asp:ListItem Value="DemographicTargeting" >Demographic Targeting</asp:ListItem>
                                <asp:ListItem Value="GeoTargeting" >Geographic Targeting</asp:ListItem>
                                </asp:DropDownList>
                                 <asp:Button ID="GetSuggestionsButton" runat="server" Text="Get Suggestions" 
                                     OnClientClick="return ShowSuggestionProgress();" OnClick="GetSuggedtionsNow" 
                                     BackColor="#FF9900" Font-Bold="True" />
                            </td>
                            </tr>
                             <tr id="Tr1" runat="server" style="min-width:600px;width: 600px;">
                            <td  colspan="4" style="height:8px;max-height:8px; " ></td>
                             </tr>                          
                            <tr id="SuggestionSelectionRow4" runat="server" style="min-width:600px;width: 600px;height:18px;">
                            <td colspan="4" id="ProgressMessageCell" runat="server"><span id="WaitMessage" class="WaitMessageHidden" >Please wait while the suggested plan is generated...<img src="images/progress16.gif" width="16" height="16" /></span></td>
                             </tr>
                             <tr id="SuggestionSelectionRow5" runat="server" style="min-width:600px;width: 600px;">
                            <td colspan="4" id="SuggestionCell" runat="server">&nbsp;</td>
                           </tr>                          
                           
                                                                        
                    </table>                
                </ContentTemplate>
                </asp:UpdatePanel>

                    <br /><br /><br />
                    <br /><br /><br />
                    <br /><br /><br />
                    <br /><br /><br />
                    <div id="Footer" style="margin-left: 130px;"><%= WebLibrary.Utils.Footer %></div>
               </td>
           
           
              <td id="HqRHCol" runat="server" rowspan="5">
                   <div id="RHDiv">

                        <br /><br />
                   <div runat="server" ID="RHInfoDiv"></div>
                       <asp:HyperLink ID="AgentLink" ImageUrl="images/USAMap_small.gif"  runat="server" 
                            href="AgentView/AgentDisplayGoogle.aspx" target="_blank" ToolTip="View Agents">View Agents</asp:HyperLink>
                       <div><em>View Agents</em></div>
                       <div runat="server" ID="EnterCompetitionDiv" ></div>
                   </div>
                </td>     
           </tr>  

       </table>
       </div>


    <div id="NameDiv">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td>Campaign::</td>
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
