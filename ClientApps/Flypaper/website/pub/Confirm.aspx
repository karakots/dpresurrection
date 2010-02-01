<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Confirm.aspx.cs" Inherits="Confirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanIt Signup Confirmation</title>
    <link href="styles/Confirm.css" rel="stylesheet" type="text/css" media="all" />
    <script language="javascript">
    function ShowMainWaitCursor()
    {
        document.getElementById( "DoSignup" ).style.visibility = "hidden";
        document.getElementById( "EditData" ).style.visibility = "hidden";
        document.getElementById( "InfoLabel" ).style.visibility = "visible";
        document.getElementById( "SignupProgressImage" ).style.visibility = "visible";
    }

    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="first_name" name="first_name" />
    <input type="hidden" runat="server" id="last_name" name="last_name" />
    <input type="hidden" runat="server" id="company" name="company" />
    <input type="hidden" runat="server" id="phone" name="phone" />
    <input type="hidden" runat="server" id="email" name="email" />
    <input type="hidden" runat="server" id="source" name="source" />
    <input type="hidden" runat="server" id="source2" name="source2" />
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div id="MainDIv"  >
           <img class="Logo" ID="Logo" src="images/Logo.gif" style="position: absolute;top: 2px; margin-left: 4px;" />    

       <div ID="NavBar" runat="server" class="NavBar">
      </div>

<br />
        <ul class="nav">
            <li></li>
            <li></li>
            <li></li>
            <li></li>
        </ul>

	 <div class="leftColumn">
	  <div class="copy" >
	  <asp:UpdatePanel runat="server" ID="InfoPanel" UpdateMode="Always">
	  <ContentTemplate>
	  <div runat="server" id="InfoDiv"></div>
	  <br />
	  <asp:ImageButton runat="server" ID="EditData" Visible="true" ImageUrl="images/Edit.gif" CssClass="submit" OnClientClick="ShowMainWaitCursor()" /> 
	  <asp:ImageButton runat="server" ID="DoSignup" Visible="false" ImageUrl="images/OKButton.gif" CssClass="submit" OnClientClick="ShowMainWaitCursor()" OnClick="OkButton_Click"/> 
	  <asp:Label runat="server" ID="InfoLabel" style="visibility:hidden;">Processing your signup...</asp:Label>
       <img ID="SignupProgressImage" name="SignupProgressImage" runat="server" width="16" height="16" src="images/progress16.gif" style="visibility:hidden; position: absolute; left:100px;margin-top:4px;margin-left:95px;"/>
	  </ContentTemplate>
	  </asp:UpdatePanel>
	  </div>
	 </div>
	 <div class="rightColumn">
	  <div runat="server" id="ConfirmDiv" style="width: 200px; background-color: #ffffff; border: 2px solid #0087a7; margin-left: auto; margin-right: auto; margin-top: 90px; visibility: hidden;">
			  <div style="padding: 5px; background-color: #0087a7; color: white; font-weight: bold; font-size: 12px; line-height: 1.4em;">You have successfully submitted your request</div>
			  <div style="padding: 15px;">
				  <p style="width: auto;">AdPlanit will respond as soon as possible.</p>
				  <p><a href="Index.aspx">Back to Signup page>></a>.</p>
			  </div>
			 </div>
      </div>    
             <div id="Footer" class="Footer">
             <table  id="Footer" class="Footer" cellpadding="0" cellspacing="0" width="350"><tr><td colspan="3" align="center">Copyright © 2008 DecisionPower, Inc. - All Rights Reserved</td></tr><tr><td width="150"><a href="PrivacyPolicy.aspx">Privacy Policy</a></td>
             <td align ="left" width="50">&nbsp;</td><td align ="right"  width="150" ><a href="TermsAndConditions.aspx">Terms and Conditions</a></td></tr></table>
	 </div>
	 </div>
	 </form>

</body>
</html>
