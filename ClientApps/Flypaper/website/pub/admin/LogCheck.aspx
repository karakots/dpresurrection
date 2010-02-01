<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogCheck.aspx.cs" Inherits="admin_LogCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit Admin - Log Details</title>
    <link rel="stylesheet" type="text/css" href="../css/LogCheck.css" />     
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3>AdPlanit User Log Details</h3><br />
    Select the month and year to view the log summary for that month.  Use the View Details menu to see detailed information for a particular user during the month.
    <br /><br />
    <h4>Month:</h4> <asp:DropDownList runat="server" ID="MonthList" AutoPostBack="true" CssClass="monthList">
    <asp:ListItem Value='1' >Jan</asp:ListItem>
    <asp:ListItem Value='2' >Feb</asp:ListItem>
    <asp:ListItem Value='3' >Mar</asp:ListItem>
    <asp:ListItem Value='4' >Apr</asp:ListItem>
    <asp:ListItem Value='5' >May</asp:ListItem>
    <asp:ListItem Value='6' >Jun</asp:ListItem>
    <asp:ListItem Value='7' >Jul</asp:ListItem>
    <asp:ListItem Value='8' >Aug</asp:ListItem>
    <asp:ListItem Value='9' >Sep</asp:ListItem>
    <asp:ListItem Value='10' >Oct</asp:ListItem>
    <asp:ListItem Value='11' >Nov</asp:ListItem>
    <asp:ListItem Value='12' >Dec</asp:ListItem>
    </asp:DropDownList>
    <h4>Year:</h4> <asp:DropDownList runat="server" ID="YearList" AutoPostBack="true" CssClass="yearList">
    <asp:ListItem >2008</asp:ListItem>
    <asp:ListItem >2009</asp:ListItem>
    <asp:ListItem >2010</asp:ListItem>
    <asp:ListItem >2011</asp:ListItem>
    </asp:DropDownList>
    <br /><br />
    <asp:Table runat="server" ID="SummaryTable" CssClass="summaryTable" CellPadding="0" CellSpacing="0" />
    <br /><br />
    <h4>View Details</h4> for: <asp:DropDownList runat="server" ID="UserList" AutoPostBack="true" CssClass="userList">
    <asp:ListItem >-</asp:ListItem>
    </asp:DropDownList>
     <br /><br />
    <asp:Table runat="server" ID="DetailsTable" CssClass="detailsTable" CellPadding="5" CellSpacing="0" />
    </div>
    </form>
</body>
</html>
