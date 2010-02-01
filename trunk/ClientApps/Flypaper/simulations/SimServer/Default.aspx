<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanitSimulation</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="ConnectBox" runat="server" ReadOnly="True" Width="300px"></asp:TextBox>
    
    </div>
    <asp:Button ID="CreateSim" runat="server" onclick="CreateSim_Click" 
        Text="Create Sim" />
    <asp:TextBox ID="SimId" runat="server" Width="200px"></asp:TextBox>
    </form>
</body>
</html>
