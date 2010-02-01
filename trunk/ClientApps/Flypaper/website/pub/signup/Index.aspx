<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AdPlanit - Where Advertising Starts</title>
    <link href="styles/AdPlanit.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../css/InfoPages.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="../AdPlanIt.js"></script>
    <script language="javascript">
//    function ShowMainWaitCursor()
//        {
//            document.body.style.cursor = "progress";
//            document.getElementById( "WaitLabel" ).style.visibility = "visible";
//            document.getElementById( "ErrorLabel" ).style.visibility = "hidden";
//        }
        function ValidateInputs() {
            if( document.getElementById( "first_name" ).value == "" || document.getElementById( "last_name" ).value == "" || document.getElementById( "email" ).value == "" ){
                alert( 'You must provide values for all fields marked with an asterisk (*).' );
                
                return false;
            }
            else {
                var em = new String();
                var em_valid = new Boolean();
                em = document.getElementById( "email" ).value;
                em_valid = false;
                
                var i1 = em.indexOf( "@", 0 );
                if( i1 >= 0 ){
                    var i2 = em.indexOf( ".", i1 );
                    if( i2 >= 0 ){
                        em_valid = true;
                    }
                }
                
                if( em_valid == false ){
                      alert( 'The email address you entered is not valid.' );
                      return false;
                }
                else {
                      return true;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" action="<%= GetProcessUrl() %>" method="post">
    <div id="MainDIv" >
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server" >
      </asp:Panel>       
<asp:Image runat="server" ID="BetaMarker" ImageUrl="../images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
       </asp:LoginView>
        <ul class="nav">
            <li></li>
            <li><a href="../Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="../Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="../Home.aspx" id="ctl00_NavHome" class="nav_home" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="../images/Logo_v2-2.gif" />
       <%-- End of standard header --%>

       	 <div class="leftColumn">
	  <div class="copy" style="margin-top: 10px;">
	   <h2>Plan, Try – <i>then Buy:</i></h2>
	   <ul>
	   <li>Use AdPlanit™, the revolutionary online planning tool, to quickly maximize the return on your advertising budget. </li> 

        <li>Bring the power available to large advertising agencies to your small or mid-sized business at no cost to you.</li>

        <li>Buy ad media with confidence knowing that you’ve optimized the mix of media to reach your prospective customers.</li>
        </ul>
	  </div>
	 </div>
	 <div class="rightColumn">
	  <div class="signupForm" style="margin-top: 10px;">
		  <b><a runat="server" id="LoginPageLink" href="../Login.aspx">Returning user? Click here to login.</a></b>
		  <h2>Learn More</h2>
                Want to know more? Click the About<br />tab. Or call, or email us at:<br /><br />
                (800) 518-9202<br />
                <a href="mailto:info@adplanit.com?subject=AdPlanit Beta">info@adplanit.com</a><br />
                <h2>Sign Up For Beta Test</h2>
                Define a new world of advertising.  No cost, obligation or sales pitch.<br /><br />
            <table cellpadding="0" cellspacing="0" id="inputTable" runat="server">		
		   <tr><td>First Name:</td><td><input type="text" name="first_name" id="first_name" runat="server"><font color="red">*</font></td></tr>
		   <tr><td>Last Name:</td><td><input type="text" name="last_name" id="last_name" runat="server"><font color="red">*</font></td></tr>
		   <tr><td>Company:</td><td><input type="text" name="company" id="company" runat="server"><font color="red"></font></td></tr>
		   <tr><td>Phone:</td><td><input type="text" name="phone" id="phone" runat="server"><font color="red"></font></td></tr>
		   <tr><td>Email Address:</td><td><input type="text" name="email" id="email" runat="server"><font color="red">*</font></td></tr>
		   <tr><td>Your Web Site:</td><td><input type="text" name="source" id="source" runat="server"></td></tr>
		   <tr><td>Where did you learn<br />about AdPlanit?:</td><td><input type="text" name="source2" id="source2" runat="server"></td></tr>
		   </table>	
		   
		   <h3 runat="server" id="ErrorLabel" >You must provide values for all fields marked with an asterisk (*)</h3>
		   <div style="float: right;">
		   <input type="image" src="images/SignupButton.gif" class="submit" onclick="if( ValidateInputs() == false )return false;" />
<%--	   <asp:ImageButton  runat="server" ID="OkButton" ImageUrl="images/SignupButton.gif" OnClick="OkButton_Clicked" CssClass="submit" OnClientClick="ShowMainWaitCursor(); return true;" />  --%>	
		   </div>
		   <h4 runat="server" id="WaitLabel" >Please wait while your information is transmitted...</h4>
		   
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
