<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Campaign.aspx.cs" Inherits="HelpPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlTolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit Campaign Definition</title>
    <link rel="stylesheet" type="text/css" href="~/css/Campaign.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input name="newPlanName" id="newPlanName" type="hidden" />
    <input name="deleteSegmentId" type="hidden" />
    <input name="deleteRegionNum" type="hidden" />
    <input name="postbackRowIndex" type="hidden" />
    <input name="postbackLinkTarget" type="hidden" />
    <input name="postbackLinkType" type="hidden" />
    <input name="AutoStartMode" ID="AutoStartMode" type="hidden" runat="server" />
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

              </td>
               <td id="RHCol" runat="server" rowspan="3" >
               <div id="RHDiv">
                <asp:ImageButton runat="server"  ImageUrl="images/Button-SaveCampaign.gif" ID="SaveCampaignButton" OnClientClick="return VerifyGoalWeightsOK();"
                       onclick="SaveButton_Click"  /><br />
                <asp:ImageButton runat="server"  ImageUrl="images/Button-CreatePlan.gif" ID="CreatePlanButton" 
                       onclick="NextButton2_Click"  /><br />
                <asp:ImageButton runat="server"  ImageUrl="images/Button-EnterPlan.gif" ID="EnterPlanButton" 
                       onclick="NextButton_Click"  /><br />
                </div>
                 <div id="Col2VideoDiv2"><br /><a href="#" onclick="OpenDemo2Window();">Building an Ad<br />Campaign</a><br /><span class="vidLabel">video</span></div>
               </td>
          </tr>
           <tr>
               <td ID="PageDataCell" runat="server" width="500" style="max-width:600px;">
               
               <% if( this.creatingNew == true ) { %>
           <h1>Create A New Campaign</h1>
           <% } else { %>
           <h1>Edit Campaign</h1>
           <% } %>
           Set the values below to describe the goals of your advertising campaign.  
           Next, build your plan by adding specific media.  You can create a media plan from scratch or have AdPlanit build one for you. <br /><br />
               
                      <div id="CampaignNameDiv" runat="server">
       
           <table width='100%' ><tr><td><asp:Label ID="CampaignLabel" runat="server" Text="<b>Name</b>. Choose a name for your Campaign:"></asp:Label>
           <asp:TextBox ID="CampaignNameTextBox" runat="server" Width="250" >My Media Campaign</asp:TextBox></td></tr></table>
           
        </div>

        
       <div id="PlanTimeDiv" runat="server">
           <asp:Label ID="Label1" runat="server" Text="Timing. " style="font-weight: bold; vertical-align:bottom"></asp:Label>
           <asp:Label ID="Label3" runat="server" Text="Start: " style="vertical-align:bottom"></asp:Label>
           <asp:TextBox ID="StartDateTextBox" runat="server" Width="90"></asp:TextBox>
          
              <ajaxControlTolkit:CalendarExtender ID="StartDateCalendar" runat="server" TargetControlID="StartDateTextBox" PopupPosition="TopLeft"
                Format="MMM dd, yyyy" ></ajaxControlTolkit:CalendarExtender> 
          
          <!--     <asp:Label ID="StartDateLabel" runat="server" Text="  "></asp:Label> 
          -->
          
          <asp:Label ID="Label8" runat="server" Text="Duration: " style="vertical-align:bottom"></asp:Label>
          <asp:TextBox ID="DurationTextBox" runat="server" Width="20">8</asp:TextBox>
          <asp:Label ID="Label9" runat="server" Text="weeks" style="vertical-align:bottom"></asp:Label>
       </div>

       <div id="PlanCostDiv" runat="server">
       <table width='100%' ><tr><td>
           <asp:Label ID="Label2" runat="server" Text="<b>Budget</b>.  Total media budget for this campaign = $"></asp:Label>
           <asp:TextBox ID="PlanCost" runat="server" Width="50" >10</asp:TextBox>
           <asp:Label ID="Label5" runat="server" Text=",000  (USD)"></asp:Label>
           </td></tr></table>
        </div>
      
       <div id="CategoryDiv">
         <b>Business Type:</b>  Choose the category and type that best describes your product or service:<br />&nbsp;&nbsp;&nbsp;&nbsp;Category:
         <asp:DropDownList runat="server" ID="CategoryList" CssClass="CategoryList" AutoPostBack="true" OnSelectedIndexChanged="Category_Changed" >
         <asp:ListItem Text="(Business Category)" ></asp:ListItem>
         </asp:DropDownList>
     
         <asp:UpdatePanel runat="server" ID="SubCategoryPanel" UpdateMode="Conditional"  RenderMode="Inline">
         <Triggers><asp:AsyncPostBackTrigger ControlID="CategoryList" />
         </Triggers>
         <ContentTemplate>
         <div id="SubCategoryDiv">
         Type:
         <img ID="SetCategoryProgressImage" name="SetCategoryProgressImage" runat="server" width="16" height="16" src="images/progress16.gif" style="visibility:hidden; position: absolute; left:0px; margin-top:5px;"/>
         <asp:DropDownList runat="server" ID="SubcategoryList" CssClass="SubcategoryList_Hidden">
         <asp:ListItem Text="(Business Type)" ></asp:ListItem>
         </asp:DropDownList>
         <asp:TextBox runat="server" ID="SubcategoryTextbox" Text="(describe your business type)" CssClass="SubcategoryTextbox_Hidden" ></asp:TextBox>
         </div>
         </ContentTemplate></asp:UpdatePanel>
       </div>
   
       <div id="PriorityDiv">
            <div id="InfoTextPanel3a" runat="server" >
                <table width='600'  cellpadding="0" cellspacing="0"><tr><td colspan="4" ><%= this.DescriptiveText3a %></td></tr>
                <tr style="font-size:8pt;"><td>&nbsp;</td><td style="padding-left:50px;padding-top:4px;">Goal</td><td style="padding-top:4px;">Weight</td> <td style="width:140px;"></td></tr>
                <tr><td style="max-width:40px;width:40px;height:24px;"><asp:CheckBox runat="server" ID="GoalSelR" Checked="true" CssClass="GoalCheckboxes"/></td>
                <td>To reach a LOT of people</td><td><asp:TextBox runat="server" ID="GoalWeightR" CssClass="GoalWeights" >20</asp:TextBox></td><td style="width:200px;"></td> </tr>
                <tr><td style="max-width:40px;width:40px;height:24px;"><asp:CheckBox runat="server" ID="GoalSelG" Checked="true" CssClass="GoalCheckboxes"/></td>
                <td>To reach people in a specific LOCATION</td><td><asp:TextBox runat="server" ID="GoalWeightG" CssClass="GoalWeights" >20</asp:TextBox></td><td style="width:200px;"></td> </tr>
                <tr><td style="max-width:40px;width:40px;height:24px;"><asp:CheckBox runat="server" ID="GoalSelP" Checked="true" CssClass="GoalCheckboxes"/></td>
                <td>To PERSUADE the people I reach</td><td><asp:TextBox runat="server" ID="GoalWeightP" CssClass="GoalWeights" >20</asp:TextBox></td><td style="width:200px;"></td> </tr>
                <tr><td style="max-width:40px;width:40px;height:24px;"><asp:CheckBox runat="server" ID="GoalSelD" Checked="true" CssClass="GoalCheckboxes"/></td>
                <td>To reach the right AGE, GENDER, etc.</td><td><asp:TextBox runat="server" ID="GoalWeightD" CssClass="GoalWeights" >20</asp:TextBox></td><td style="width:200px;"></td> </tr>
                <tr><td style="max-width:40px;width:40px;height:24px;"><asp:CheckBox runat="server" ID="GoalSelT" Checked="true" CssClass="GoalCheckboxes"/></td>
                <td>To reach people at the right TIME</td><td><asp:TextBox runat="server" ID="GoalWeightT" CssClass="GoalWeights" >20</asp:TextBox></td><td ID="TotalGoalWeight" style="width:200px; text-align:right"><span runat="server" id="TotalGoalWeightSpan">Total: 100</span></td> </tr>
                </table>              
            </div>
            

       </div>



       <div ID="SliderBkgDiv1">
       <asp:Label ID="SliderTiitle1" Text="<b>Purchase Interval</b>. Set the slider to describe how often a given consumer purchases your product or service." runat="server"></asp:Label> 
      <ajaxControlTolkit:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="purchaseCycleTextBox" Maximum="16" Minimum="1">
      </ajaxControlTolkit:SliderExtender>
       <asp:Label ID="SliderLabel1" Text="Daily" runat="server"></asp:Label> 
       <asp:Label ID="SliderLabel2" Text="Weekly" runat="server"></asp:Label> 
       <asp:Label ID="SliderLabel3" Text="Monthly" runat="server"></asp:Label> 
       <asp:Label ID="SliderLabel4" Text="Yearly" runat="server"></asp:Label> 
       <asp:Label ID="SliderLabel5" Text="Longer" runat="server"></asp:Label> 
      <div ID="SliderDiv1"><asp:TextBox ID="purchaseCycleTextBox" runat="server">9</asp:TextBox></div>
      </div>
      <div ID="TimInConsidDiv"><b>Time in Consideration.</b>  % of time your customers are considering a purchase of your product: <asp:TextBox ID="ConsiderationBox" runat="server">0</asp:TextBox>%</div>
      </div>
      <div ID="PrevSpendingDiv"><b>Previous Spending.</b>  Total amount spent on media in the past 12 months: $<asp:TextBox ID="PrevSpendingBox" runat="server">0</asp:TextBox>,000  (USD)</div>
      </div>
      <div ID="CurrConversionsDIv"><b>Tracking Data.</b>  In the past 30 days there have been <asp:TextBox ID="CurrConversionBox" runat="server">9</asp:TextBox> 
      <asp:DropDownList runat="server" ID="ConversionUnits">
      <asp:ListItem>Sales</asp:ListItem>
      <asp:ListItem>Customers</asp:ListItem>
      <asp:ListItem>Leads</asp:ListItem>
      </asp:DropDownList> </div>
      </div>
                   <table cellpadding="3" cellspacing="0" id="SegListTable" class="DemoInfoTable">
                       <tr>
                           <td colspan="2">
                               <asp:Label ID="DemoTitle" Text="<b>Customer Demographics</b>. Describe your target customers:"
                                   runat="server"></asp:Label>
                           </td>
                       </tr>
                       <% if( this.currentMediaPlan.DemographicCount == 0 && editMode == false ) { %>
                       <tr>
                           <td colspan="2" class="demoName">
                               <a  href='#' onclick='__doPostBack( "AddSegment" ); return false;'  >Define a Target Segment</a>
                           </td>
                       </tr>
                       <% }
          else { %>
                       <% for( int s = 0; s < this.currentMediaPlan.DemographicCount; s++ ) {%>
                       <tr>
                           <td class="demoName">
                               <% if( editMode == false ) { %>
                                   <a  href='#' onclick='<%= EditSegmentLink( s ) %>'>
                                   <%= this.currentMediaPlan.Specs.Demographics[ s ].DemographicName.Trim()%> 
                                   </a>
                                   &nbsp;&nbsp;&nbsp;
                                    <a href='#' onclick='<%= RemoveSegmentLink( this.currentMediaPlan.Specs.Demographics[ s ].DemographicID ) %>'>(remove)</a>
                               <% }
                                  else { %>
                                  <%= this.currentMediaPlan.Specs.Demographics[ s ].DemographicName.Trim()%>
                               <% } %>
                           </td>
                       </tr>
                       <% } %>
                       <% } %>
                       <% if( this.currentMediaPlan.DemographicCount > 0 && editMode == false ) { %>
                       <tr>
                           <td style="padding-bottom: 4px" colspan="2">
                               <a  href='#' onclick='__doPostBack( "AddSegment" ); return false;'  >Add Another Segment</a>
                           </td>
                       </tr>
                       <% } %>
               </td>
               <td id="Td1" runat="server" width="500">
                   <table cellpadding="3" cellspacing="0" id="GeoListTable" class="GeoInfoTable">
                       <tr>
                           <td colspan="2" class="geoInfo">
                               <asp:Label ID="Label4" Text="<b>Geographics</b>. Specify your target market region(s):"
                                   runat="server"></asp:Label>
                           </td>
                       </tr>
                       <% if( this.currentMediaPlan.Specs.GeoRegionNames.Count == 0 && editMode == false ) { %>
                       <tr>
                           <td class="geoInfo" style="padding-left: 24px;" >
                               <a  href='#' onclick='__doPostBack( "AddRegion" ); return false;'  >Choose a Region</a>
                           </td>
                       </tr>
                       <% }
          else { %>
                       <% for( int s = 0; s < this.currentMediaPlan.Specs.GeoRegionNames.Count; s++ ) { %>
                       <tr>
                           <td class="geoName" style="padding-left: 24px;">
                               <%= this.currentMediaPlan.Specs.GeoRegionNames[ s ].Trim()%>&nbsp;&nbsp;&nbsp;
                                  <% if( editMode == false ) { %>
                                        <a  href='#' onclick='<%= RemoveRegionLink( s ) %>'>(remove)</a>
                                  <% } %>

                           </td>
                       </tr>
                       <% } %>
                       <% } %>
                       <% if( this.currentMediaPlan.Specs.GeoRegionNames.Count > 0 && editMode == false ) { %>
                       <tr>
                           <td style="padding-bottom: 4px" colspan="2"  class="geoInfo">
                                  <a  href='#' onclick='__doPostBack( "AddRegion" ); return false;'  >Add Another Region</a>
                           </td>
                       </tr>
                       <% } %>
                       <%-- - - - - - - -  ALL ITEMS BELOW DEMOGRAPHCS MUST BE INSIDE THIS TABLE TO ALLOW FOR HEIGHT GROWTH  - - - - - - --%>
                       <tr>
                           <td>
                               <br />
                               <br />
                               <div id="Div1" runat="server" style="font-size: 7pt; margin-left: 200px;">
                                   <%= WebLibrary.Utils.Footer %></div>
                               <asp:Label runat="server" ID="DebugInfo" Text=""></asp:Label>
                           </td>
                       </tr>
                       <%-- - - - - - - -  END OF BELOW-DEMOGRAPHICS AREA - - - - - - --%>
                   </table>
               </td>
           </tr>
        </table>
           </td> </tr>
           <tr>
               <td id="FooterCell" align="center">
                   <table id="FooterTable" align="center">
                       <tr>
                           <td id="Footer">
                           </td>
                       </tr>
                   </table>
               </td>
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
            <p id="PopupCaptionTextP" class="PopupCaptionTextP" runat="server" >Enter new name for the plan:</p> <input type="text" id="NewNameBox"  class="NewNameBoxShown" runat="server" /> 
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
</body>
</html>
