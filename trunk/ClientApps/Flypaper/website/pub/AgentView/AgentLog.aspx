<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentLog.aspx.cs" Inherits="AgentView_AgentLog" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agent Log</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      <asp:Panel ID="AgentHeaderPanel" runat="server" style="cursor: pointer;">
        <div>
               <asp:ImageButton ID="Agent_ToggleImage" runat="server" ImageUrl="../images/collapse.jpg" AlternateText="collapse" />
            <em> <b>Agent Household Information</b></em>
        </div>
    </asp:Panel>
    <asp:Panel ID="AgentInfoPanel" runat="server">
     <div id="AgentInfo" runat="server"></div>
    </asp:Panel>
    
    <asp:Panel ID="AgentMediaHedader" runat="server" Style="cursor: pointer;">
        <div>
            <asp:ImageButton ID="Agentmedia_ToggleImage" runat="server" ImageUrl="../images/collapse.jpg"
                AlternateText="collapse" />
            <em><b>Agent Media Information</b></em>
        </div>
    </asp:Panel>
    <asp:Panel ID="AgentMediaInfoPanel" runat="server">
        <div id="AgentMediaInfo" runat="server">
        </div>
    </asp:Panel>
  
    <div id="LogDiv" runat="server">
    </div>
        
     <ajaxToolkit:CollapsiblePanelExtender ID="cpeDesc" runat="Server"
        TargetControlID="AgentInfoPanel"
        ExpandControlID="AgentHeaderPanel"
        CollapseControlID="AgentHeaderPanel"
        Collapsed="True"
        ImageControlID="Agent_ToggleImage"
        ExpandedText="(Hide Details...)"
        CollapsedText="(Show Details...)"
        ExpandedImage="../images/collapse.jpg"
        CollapsedImage="../images/expand.jpg"
        SuppressPostBack="true" />
          
        <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server"
        TargetControlID="AgentMediaInfoPanel"
        ExpandControlID="AgentMediaHedader"
        CollapseControlID="AgentMediaHedader"
        Collapsed="True"
        ImageControlID="Agentmedia_ToggleImage"
        ExpandedText="(Hide Details...)"
        CollapsedText="(Show Details...)"
        ExpandedImage="../images/collapse.jpg"
        CollapsedImage="../images/expand.jpg"
        SuppressPostBack="true" />
        
    </form>
</body>
</html>
